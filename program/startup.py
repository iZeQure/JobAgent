import logging as log
from program.modules.services.data_service import DataService
from program.modules.services.algorithm_service import AlgorithmService
from program.modules.providers.web_data_provider import WebDataProvider
from program.modules.providers.vacant_job_algorithm_provider import VacantJobSearchAlgorithmProvider
from program.modules.providers.job_advert_search_algorithm_provider import JobAdvertSearchAlgorithmProvider
from program.modules.managers.database_manager import DatabaseManager
from program.modules.database.db import DatabaseMSSQL


class Startup(object):
    __CONFIGURATION_MODE = "DEVELOPMENT"

    __app_config: object
    __company_list: []
    __existing_job_advert_list: []
    __data_service: DataService
    __algorithm_service: AlgorithmService
    __web_driver_provider: WebDataProvider

    def __init__(self, app_config: object):
        # Set the app config as a global variable.
        self.__app_config = app_config
        self.__build_config()

    def initialize_zombie(self):
        """
        Initialize the zombie, to crawl on the web.
        """
        try:
            # Crawler Information
            # Gathers the information from the database, to display information on execution.

            log.info('Initializing Crawler..')
            initialized_information = self.__data_service.get_crawler_information()

            # Validate the initialization progress.
            if initialized_information[0] is False:
                raise ValueError('Initializing Failed.')
            else:
                # Data Crawling
                # 1. Compiles a list of vacant job urls per company job page url.
                # 2. Compiles the list of vacant jobs into jobadvert.

                info_message = initialized_information[1]
                version_message = initialized_information[3]
                responsibility_message = initialized_information[2]
                log.info(f'{info_message} started running as {responsibility_message} on {version_message}')

                self.__compile_vacant_job_data_information()
                self.__compile_job_advert_data_information()

        except ValueError as er:
            log.exception(er)
        except Exception as ex:
            log.exception(ex)

    def __compile_job_advert_data_information(self):
        try:
            # Get list of available vacant jobs.
            vacant_job_data_list = self.__data_service.get_vacant_jobs()

            # Check that vacant jobs is defined.
            if vacant_job_data_list is not None:
                # Validate the length of the list.
                if len(vacant_job_data_list) == int(0):
                    raise ValueError('Vacant Job List was empty.')
                else:
                    # Get the page sources from the vacant job list.
                    vacant_jobs = self.__algorithm_service.get_html_page_source_data_list(vacant_job_data_list)

                    # Create a list for job advert data.
                    jobadvert_data_list = self.__get_jobadvert_data_from_list(vacant_jobs)

                    self.__save_jobadvert_data_information(jobadvert_data_list)
            else:
                raise Exception('Vacant job list was None.')
        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __compile_vacant_job_data_information(self):
        # Flow
        # 1. Creates a list of companies then stored into a list.
        # 2. Create a temporary list of vacant jobs, by found vacant jobs within the company job page url.
        # 3. When all companies are checked, save the vacant job list, from found vacant jobs, into the database.

        try:
            company_list = self.__data_service.get_companies()

            if company_list is None:
                raise Exception('Invalid data list, no companies could be initialized.')
            else:
                if len(company_list) == int(0):
                    raise ValueError('No data was found in the company data list.')
                else:
                    try:
                        temp_data_list = []

                        for company in company_list:
                            # make an algorithm that compiles a list of links from the company job page url.
                            # then to store the list in the temporary list for every company found.
                            log.info('Not Implemented')
                            break

                        if temp_data_list is None:
                            raise Exception('Temp data list was not defined.')
                        else:
                            if len(temp_data_list) == int(0):
                                raise ValueError('No data found within the temp data list.')
                            else:
                                # Store the found vacant job links in the database.
                                log.info('Not Implemented')
                    except ValueError as er:
                        log.warning(er)
                    except Exception as ex:
                        log.exception(ex)
        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __get_jobadvert_data_from_list(self, data: []):
        # Create a job advert data list.
        jobadvert_data_list = []

        try:
            # Check the data list is None.
            if data is None:
                raise Exception('Parameter was not defined.')
            else:
                try:
                    # check the length of the data list is valid.
                    if len(data) == 0:
                        raise Exception('Parameter was empty.')
                    else:
                        for data in data:
                            jobadvert = self.__algorithm_service.compile_jobadvert_data_object(data)
                            jobadvert_data_list.append(jobadvert)

                        return jobadvert_data_list
                except Exception as ex:
                    log.exception(ex)
        except Exception as ex:
            log.exception(ex)

    def __save_jobadvert_data_information(self, jobadvert_data_list: []) -> None:
        try:
            existing_jobadvert_data_list = self.__data_service.get_existing_job_adverts()

            # Validate the data list for None.
            if jobadvert_data_list is not None and existing_jobadvert_data_list is not None:
                try:
                    if len(jobadvert_data_list) == int(0) and len(existing_jobadvert_data_list) == int(0):
                        raise ValueError('No data found in the given lists.')
                    else:
                        # logging.info(f"Saving <{len(jobadvert_data_list)}> compiled job advert(s).")
                        log.info('Looking for duplicate data.')

                        # Validate duplicate information.
                        for existing_data in existing_jobadvert_data_list:
                            for jobadvert in jobadvert_data_list:
                                if existing_data[0] == jobadvert.id:
                                    log.info(f'Found duplicate data on -> {existing_data[0]}')
                                    # Update jobadvert with new information.
                                    self.__data_service.update_job_advert(jobadvert)
                                else:
                                    log.info(
                                        f'No duplicate found on [{existing_data[0]}], creating jobadvert -> {jobadvert.id}')
                                    # Create a jobadvert with new information.
                                    self.__data_service.create_job_advert(jobadvert)
                except ValueError as er:
                    log.warning(er)
            else:
                raise Exception('Given data to save was invalid.')
        except Exception as ex:
            log.exception(ex)
        finally:
            log.info('Finished [Save JobAdvert Data Information].')

    def __build_config(self):
        try:
            # Build logging configuration.
            log.info('Building logging configuration..')
            log_config = self.__app_config['Logging']
            log.basicConfig(
                level=log_config['Level']['INFO'],
                format=log_config['Format'],
                datefmt=log_config['DateFormat']
            )
        except Exception as ex:
            log.exception(ex)

        try:
            # Make Config from Environment
            config = self.__app_config["ENVIRONMENT"][self.__CONFIGURATION_MODE]

            # Build service collection.
            log.info('Building service collection..')

            database = DatabaseMSSQL(config)
            manager = DatabaseManager(database)
            self.__data_service = DataService(manager)
            job_provider = JobAdvertSearchAlgorithmProvider(self.__data_service)
            vacant_provider = VacantJobSearchAlgorithmProvider(self.__data_service)

            self.__web_driver_provider = WebDataProvider(config)
            self.__algorithm_service = AlgorithmService(
                job_provider,
                vacant_provider,
                self.__web_driver_provider
            )
        except Exception as ex:
            log.exception(ex)
