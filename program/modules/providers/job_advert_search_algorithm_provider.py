import logging as log
from re import compile, match, IGNORECASE
from datetime import datetime

from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.services.data_service import DataService
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob
from program.modules.objects.address import Address


class JobAdvertSearchAlgorithmProvider(SearchAlgorithmProvider):
    __not_found_text = "Ikke Fundet"
    __not_found_id = 0

    __title_filter_keys: []
    __email_filter_keys: []
    __phone_number_filter_keys: []
    __description_filter_keys: []
    __location_name_filter_keys: []
    __registration_date_name_filter_keys: []
    __deadline_date_name_filter_keys: []
    __category_filter_keys: []
    __specialization_filter_keys: []

    def __init__(self, data_service: DataService):
        super().__init__(data_service)

        self.__load_filters()

    def __load_filters(self):
        service = self.data_service

        self.__title_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Title_Key"))
        self.__email_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Email_Key"))
        self.__phone_number_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Phone_Number_Key"))
        self.__description_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Description_Key"))
        self.__location_name_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Location_Key"))
        self.__registration_date_name_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Registration_Date_Key"))
        self.__deadline_date_name_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Deadline_Date_Key"))
        self.__category_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Category_Key"))
        self.__specialization_filter_keys = \
            self.__get_keys_from_list(service.get_algorithm_keywords_by_key("Specialization_Key"))

    def __find_title(self) -> str:
        for arg in self.__title_filter_keys:
            title = self.soup.find(text=compile(arg, flags=IGNORECASE))

            if title is not None:
                return title

        return self.__not_found_text

    def __find_email(self) -> str:
        for arg in self.__email_filter_keys:
            mail = self.soup.select_one(arg)

            if mail is not None:
                return mail.get_text()

        return self.__not_found_text

    def __find_phone_number(self) -> str:
        regex = "([0-9]{8,8})"

        for arg in self.__phone_number_filter_keys:
            for result in self.soup.find_all(arg):
                text_from_result = result.get_text()

                if text_from_result is not None:
                    if match(pattern=regex, string=str(text_from_result)):
                        return text_from_result

        return self.__not_found_text

    def __find_description(self) -> str:
        for arg in self.__description_filter_keys:
            description = self.soup.select_one(arg)

            if description is not None:
                return description.get_text(' ', True)

        return self.__not_found_text

    def __find_location(self) -> str:
        found_location_element = None
        elements = []

        for location_filter in self.__location_name_filter_keys:
            elements.append(self.soup.find(text=compile(location_filter, flags=IGNORECASE)))

        for arg in elements:
            if arg is not None:
                found_location_element = arg

        if found_location_element is not None:
            actual_location = found_location_element.parent.nextSibling
            if actual_location is not None:
                return actual_location.get_text(separator=' ')

        return self.__not_found_text

    def __find_registration_date(self) -> datetime:
        reg_datetime = None
        date_elements = []
        for date_filter in self.__registration_date_name_filter_keys:
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
        # date_time_str = self.get_actual_result_from_element(
        #     self.get_date_elements(
        #         self._deadline_date_name_filter_keys
        #     )
        # )
        #
        # date_time_obj = datetime.strptime(date_time_str, '%d.%m.%Y')
        # return date_time_obj

    def __find_category(self) -> int:
        category_filter_keys = self.__category_filter_keys
        categories = self.data_service.get_categories()
        category_id = 0
        result = ""

        for category_filter_key in category_filter_keys:
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
        else:
            return self.__not_found_id

    def __find_specialization(self) -> int:
        specialization_filter_keys = self.__specialization_filter_keys
        specializations = self.data_service.get_specializations()
        specialization_id = 0

        for specialization_filter in specialization_filter_keys:
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

    def get_data(self, vacant_job: object) -> JobAdvert:
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
                log.info(f'Processing gatherer with [{vacant_job.id}] for company [{vacant_job.company_id}].')

                # Set the page source of the current vacant job.
                self.set_page_source(vacant_job.html_page_source)

                # Process the search algorithm.
                job_advert = JobAdvert(
                    vacant_job_id=vacant_job.id,
                    category_id=self.__find_category(),
                    specialization_id=self.__find_specialization(),
                    title=self.__find_title(),
                    summary='None',
                    description=self.__find_description(),
                    email=self.__find_email(),
                    phone_number=self.__find_phone_number(),
                    registered_date_time=self.__find_registration_date(),
                    application_deadline_date_time=self.__find_deadline_date(),
                    address=Address(
                        job_advert_vacant_job_id=vacant_job.id,
                        street_address=self.__find_location(),
                        city='None',
                        country='None',
                        postal_code='None'
                    )
                )

                return job_advert
            else:
                raise TypeError('Given type was not of type VacantJob.')

    @staticmethod
    def __get_keys_from_list(keys: []) -> []:
        temp_list = []
        for key in keys:
            temp_list.append(key[0])
        return temp_list
