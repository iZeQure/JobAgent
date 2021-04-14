import logging
import traceback

from DataLayer import providers as provider, services as service, access as manager, database


class Startup:
    __SET_CONFIGURATION_MODE = "DEVELOPMENT"

    __app_config: object

    __company_list: []
    __existing_jobadvert_list: []

    __database: database.DatabaseMSSQL
    __manager: manager.DataManager
    __data_service: service.DataService
    __algorithm_service: service.AlgorithmService
    __gecko_driver_provider: provider.PageSourceProvider

    def __init__(self, app_config: object):
        # Initialize logging config.
        logging.basicConfig(level=app_config["Logging"]["Level"]["INFO"],
                            format=app_config["Logging"]["Format"],
                            datefmt=app_config["Logging"]["DateFormat"])

        self.__app_config = app_config["ENVIRONMENT"][self.__SET_CONFIGURATION_MODE]

    def init_crawler(self) -> None:
        """
        Initializes the Crawler.
        """

        # Initialize Services
        # Creates the necessary services and instantiates them.

        logging.info("Starting Services..")
        self.__initialize_services()

        try:
            # Crawler Information
            # Gathers the information from the database, to display information on execution.

            logging.info('Initializing Crawler..')
            initialized_information = self.__data_service.get_crawler_information()

            # Validate the initialization progress.
            if initialized_information[0] is False:
                logging.error('Initializing Failed.')
            else:
                # Data Crawling
                # 1. Compiles a list of vacant job urls per company job page url.
                # 2. Compiles the list of vacant jobs into jobadvert.

                info_message = initialized_information[1]
                version_message = initialized_information[3]
                responsibility_message = initialized_information[2]
                logging.info(f'{info_message} started running as {responsibility_message} on {version_message}')

                self.__compile_vacant_job_data_information()
                self.__compile_jobadvert_data_information()

        except ValueError:
            logging.exception("Error: 40 - Uncaught Exception.")
        except Exception as ex:
            logging.exception(ex)

    def __initialize_services(self) -> None:
        try:
            self.__database = database.DatabaseMSSQL(self.__app_config)
            self.__manager = manager.DataManager(self.__database)
            self.__data_service = service.DataService(self.__manager)

            jobadvert_search_algorithm = provider.JobAdvertSearchAlgorithmProvider(self.__data_service)
            vacantjob_search_algorithm = provider.VacantJobSearchAlgorithmProvider(self.__data_service)
            gecko_driver_provider = provider.PageSourceProvider(self.__app_config)

            self.__algorithm_service = service.AlgorithmService(
                jobadvert_search_algorithm,
                vacantjob_search_algorithm,
                gecko_driver_provider)

        except Exception as ex:
            logging.exception(ex)

    def __compile_jobadvert_data_information(self) -> None:
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
            logging.warning(er)
        except Exception as ex:
            logging.exception(ex)

    def __compile_vacant_job_data_information(self) -> None:
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
                            logging.info('Not Implemented')
                            break

                        if temp_data_list is None:
                            raise Exception('Temp data list was not defined.')
                        else:
                            if len(temp_data_list) == int(0):
                                raise ValueError('No data found within the temp data list.')
                            else:
                                # Store the found vacant job links in the database.
                                logging.info('Not Implemented')
                    except ValueError as er:
                        logging.warning(er)
                    except Exception as ex:
                        logging.exception(ex)
        except ValueError as er:
            logging.warning(er)
        except Exception as ex:
            logging.exception(ex)

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
                        logging.info('Looking for duplicate data.')

                        # Validate duplicate information.
                        for existing_data in existing_jobadvert_data_list:
                            for jobadvert in jobadvert_data_list:
                                if existing_data[0] == jobadvert.id:
                                    logging.info(f'Found duplicate data on -> {existing_data[0]}')
                                    # Update jobadvert with new information.
                                    self.__data_service.update_jobadvert(jobadvert)
                                else:
                                    logging.info(f'No duplicate found on [{existing_data[0]}], creating jobadvert -> {jobadvert.id}')
                                    # Create a jobadvert with new information.
                                    self.__data_service.create_jobadvert(jobadvert)
                except ValueError as er:
                    logging.warning(er)
            else:
                raise Exception('Given data to save was invalid.')
        except Exception as ex:
            logging.exception(ex)
        finally:
            logging.info('Finished [Save JobAdvert Data Information].')

    def __get_jobadvert_data_from_list(self, data_list: []) -> []:
        # Create a job advert data list.
        jobadvert_data_list = []

        try:
            # Check the data list is None.
            if data_list is None:
                raise Exception('Parameter was not defined.')
            else:
                try:
                    # check the length of the data list is valid.
                    if len(data_list) == 0:
                        raise Exception('Parameter was empty.')
                    else:
                        for data in data_list:
                            jobadvert = self.__algorithm_service.compile_jobadvert_data_object(data)
                            jobadvert_data_list.append(jobadvert)

                        return jobadvert_data_list
                except Exception as ex:
                    logging.error(traceback.TracebackException)
        except Exception as ex:
            logging.error(traceback.TracebackException)
