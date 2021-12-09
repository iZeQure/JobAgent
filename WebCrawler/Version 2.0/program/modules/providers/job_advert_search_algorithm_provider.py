import logging as log
from datetime import datetime
from re import compile, match, IGNORECASE

from program.modules.managers.database_manager import DatabaseManager
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.providers.web_data_provider import WebDataProvider


class JobAdvertSearchAlgorithmProvider(SearchAlgorithmProvider):
    __not_found_text = "Ikke Fundet"
    __not_found_id = 0

    __filters: []

    __title_filter_keys: []
    __email_filter_keys: []
    __phone_number_filter_keys: []
    __description_filter_keys: []
    __location_name_filter_keys: []
    __registration_date_name_filter_keys: []
    __deadline_date_name_filter_keys: []
    __category_filter_keys: []
    __specialization_filter_keys: []

    def __init__(self, manager: DatabaseManager, web_data: WebDataProvider):
        super().__init__(manager, web_data)

        self.__load_filters()

    def run_algorithm(self):
        try:
            # Get list of available vacant jobs.
            vacant_job_data_list = self.manager.get_vacant_jobs()

            # Check that vacant jobs is defined.
            if vacant_job_data_list is not None:
                # Validate the length of the list.
                if len(vacant_job_data_list) == int(0):
                    raise ValueError('Vacant Job List was empty.')
                else:
                    # Get the page sources from the vacant job list.
                    vacant_jobs = self.web_data.load_page_sources(vacant_job_data_list)
                    # Create a list for job advert data.
                    jobadvert_data_list = self.__extract_job_advert_data_information(vacant_jobs)
                    self.__handle_jobadvert_data_information(jobadvert_data_list)
            else:
                raise Exception('Vacant job list was None.')
        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __load_filters(self):
        self.__filters = self.manager.get_filter_keys()

    def __get_filters_by_key(self, key: str):
        return [(x[1]) for x in self.__filters if x[0] == key]

    def __find_title(self) -> str:
        for t_filter in self.__get_filters_by_key('title'):
            title = self.soup.find(text=compile(t_filter, flags=IGNORECASE))

            if title is not None:
                return title

        return self.__not_found_text

    def __find_email(self) -> str:
        for e_filter in self.__get_filters_by_key('email'):
            email = self.soup.select_one(e_filter)

            if email is not None:
                return email.get_text()

        return self.__not_found_text

    def __find_phone_number(self) -> str:
        regex = "([0-9]{8,8})"
        for pnr_filter in self.__get_filters_by_key('phone_number'):
            for result in self.soup.find_all(pnr_filter):
                text_from_result = result.get_text()
                if text_from_result is not None:
                    if match(pattern=regex, string=str(text_from_result)):
                        return text_from_result

        return self.__not_found_text

    def __find_description(self) -> str:
        for d_filter in self.__get_filters_by_key('description'):
            description = self.soup.select_one(d_filter)

            if description is not None:
                return description.get_text(' ', True)

        return self.__not_found_text

    # def __find_location(self) -> str:
    #     found_location_element = None
    #     elements = []
    #
    #     for l_filter in self.__get_filters_by_key(''):
    #         elements.append(self.soup.find(text=compile(l_filter, flags=IGNORECASE)))
    #
    #     for arg in elements:
    #         if arg is not None:
    #             found_location_element = arg
    #
    #     try:
    #         if found_location_element is not None:
    #             actual_location = found_location_element.parent.nextSibling
    #             if actual_location is not None:
    #                 return actual_location.get_text(separator=' ')
    #     except AttributeError:
    #         return self.__not_found_text
    #
    #     return self.__not_found_text

    def __find_registration_date(self) -> datetime:
        reg_datetime = None
        date_elements = []
        for date_filter in self.__get_filters_by_key('registration_datetime'):
            date_elements.append(self.soup.find(text=compile(date_filter), flags=IGNORECASE))

        if date_elements is not None:
            for date in date_elements:
                if date is not None:
                    if date.has_attr('datetime'):
                        reg_datetime = date['datetime']

        if reg_datetime is not None:
            reg_datetime = datetime.strptime(reg_datetime, '%d.%m.%Y')
        else:
            reg_datetime = datetime.today()

        return reg_datetime

    def __find_deadline_date(self) -> datetime:
        return datetime.today()

    def __find_category(self) -> int:
        categories = self.manager.get_categories()
        category_id = 0
        result = ""

        for category_filter_key in self.__get_filters_by_key('category'):
            result = self.soup.find(text=compile(category_filter_key, flags=IGNORECASE))
            if result is not None:
                result = result.lower()

        if result is not None:
            for i, category in enumerate(categories):
                if str(category[1]).lower() in result:
                    category_id = category[0]
                    break

            if category_id != 0:
                return category_id

            return self.__not_found_id
        else:
            return self.__not_found_id

    def __find_specialization(self) -> int:
        specializations = self.manager.get_specializations()
        specialization_id = 0

        for specialization_filter in self.__get_filters_by_key('specialization'):
            result = self.soup.find(text=compile(specialization_filter, flags=IGNORECASE))
            if result is not None:
                result = result.lower()
                for specialization in specializations:
                    lowered_spec = str(specialization[1])
                    if lowered_spec.lower() in result:
                        specialization_id = specialization[0]
                        break
            else:
                continue

        if specialization_id != 0:
            return specialization_id

        return self.__not_found_id

    def __scrape_job_advert_data(self, vacant_job: object) -> JobAdvert:
        """
        Gathers information for the JobAdvert specified by the VacantJob.
        Args:
            vacant_job: A given object to specify where to gather from.
        Returns: A JobAdvert containing the information from the given vacant job,
        if it's instance of VacantJob otherwise None.

        """
        if vacant_job is None:
            raise ValueError('Parameter was type of None.')
        else:
            if isinstance(vacant_job, VacantJob):
                # Log gathering data information.
                # log.info(f'Processing gatherer with [{vacant_job.id}] for company [{vacant_job.company_id}].')

                # Set the page source of the current vacant job.
                self.initialize_soup(vacant_job.get_page_source)

                # Process the search algorithm.
                job_advert = JobAdvert(
                    vacant_job_id=vacant_job.get_entity_id,
                    category_id=self.__find_category(),
                    specialization_id=self.__find_specialization(),
                    title=self.__find_title(),
                    summary='None',
                    description=self.__find_description(),
                    email=self.__find_email(),
                    phone_number=self.__find_phone_number(),
                    registered_date_time=self.__find_registration_date(),
                    application_deadline_date_time=self.__find_deadline_date(),
                )

                return job_advert
            else:
                raise TypeError('Given type was not of type VacantJob.')

    def __extract_job_advert_data_information(self, data_list: [VacantJob]):
        # Create a job advert data list.
        jobadvert_data_list = []

        # Check the data list is None.
        if data_list is None:
            raise Exception('Parameter was not defined.')
        else:
            # check the length of the data list is valid.
            if len(data_list) == 0:
                raise Exception('Parameter was empty.')
            else:
                for data in data_list:
                    jobadvert = self.__scrape_job_advert_data(data)
                    jobadvert_data_list.append(jobadvert)

                return jobadvert_data_list

    def __handle_jobadvert_data_information(self, jobadvert_data_list: []) -> None:
        if jobadvert_data_list is None:
            raise ValueError(f'Parameter {type(jobadvert_data_list)} was not defined.')

        if len(jobadvert_data_list) == int(0):
            raise ValueError(f'The length of {type(jobadvert_data_list)} was {len(jobadvert_data_list)}.')

        log.info(f'Handling <{len(jobadvert_data_list)}> Job Adverts.')
        for job in jobadvert_data_list:
            try:
                if self.__job_advert_exists(job):
                    # log.info(f'Updating data for [{job.id}].')
                    self.manager.update_job_advert(job)
                else:
                    # log.info(f'Creating new data for [{job.id}]')
                    self.manager.create_job_advert(job)
            except ValueError as err:
                log.warning(err)
                continue

    def __job_advert_exists(self, job_advert: JobAdvert) -> bool:
        if not isinstance(job_advert, JobAdvert):
            raise ValueError(f'Parameter [job_advert] was not type of {type(JobAdvert)}')

        existing_job_adverts = set(self.manager.get_job_adverts())
        if existing_job_adverts is None:
            raise ValueError(f'Existing Job Advert were not defined.')
        else:
            if len(existing_job_adverts) == int(0):
                return False

            if not any(job_advert_id == job_advert.get_entity_id for job_advert_id in existing_job_adverts):
                # log.info(f'No duplicate found for [{job_advert.id}].')
                return False

            # log.info(f'Found duplicate data at [{job_advert.id}].')
            return True

    @staticmethod
    def __get_keys_from_list(keys: []) -> []:
        temp_list = []
        for key in keys:
            temp_list.append(key[0])
        return temp_list
