import logging
import time
import DataLayer.database as database
import DataLayer.access as manager
import DataLayer.services as service
import DataLayer.providers as provider


class Startup:
    __database: database.DatabaseMSSQL
    __manager: manager.DataManager
    __data_service: service.DataService
    __algorithm_service: service.AlgorithmService
    __gecko_driver_provider: provider.GeckoDriverProvider
    __algorithm_provider: provider.SearchAlgorithmProvider

    def __init__(self):
        logging.basicConfig(level=logging.INFO,
                            format='%(asctime)s [%(levelname)s] - %(message)s',
                            datefmt='%m/%d/%Y %I:%M:%S %p')

    def start_crawling(self):
        """
            Starts Job Agent Crawler
        """

        """ Initialize Services """
        logging.info("Starting Services..")
        self.__initialize_services()

        try:
            logging.info('Initializing Crawler..')
            initialized_information = self.__data_service.initialize_crawler()

            """ Shutdown Crawler, if init failed. """
            if initialized_information[0] is False:
                logging.error('Initializing Failed.')
            else:
                logging.info(f'{initialized_information[1]} Initialized on v{initialized_information[3]}')
                logging.info(f'Responsibility: {initialized_information[2]}')

                self.__crawl_page_urls_for_vacant_job_links()

                self.__compile_job_adverts_by_found_source_links()

        except ValueError:
            logging.exception("Error: 40 - Uncaught Exception.")

    def __compile_job_adverts_by_found_source_links(self) -> None:
        source_links = self.__data_service.get_source_links()
        try:
            if len(source_links) != 0:
                logging.info("Acquiring Source Data..")
                raw_data = self.__algorithm_service.get_raw_jobadvert_source_data(source_links=source_links)

                if len(raw_data) != 0:
                    jobadvert_data_list = []
                    logging.info(f"Compiling Job Adverts..")
                    for raw in raw_data:
                        jobadvert_data_list.append(
                            self.__algorithm_service.find_jobadvert_match_from_raw_source_data(raw))

                    if len(jobadvert_data_list) != 0:
                        logging.info(f"Saving <{len(jobadvert_data_list)}> compiled job advert(s).")
                        for dataset in jobadvert_data_list:
                            self.__data_service.create_jobadvert(dataset)
                    else:
                        logging.info('No job adverts were compiled.')
                else:
                    logging.info('No Data was found.')
            else:
                logging.info('No Source Links were found.')
        finally:
            logging.warning('Exiting in 10 seconds.')
            time.sleep(10)

    def __crawl_page_urls_for_vacant_job_links(self) -> None:
        logging.info('Getting necessary data for web crawling.')
        company_list = self.__data_service.get_companies()
        vacant_job_list = self.__data_service.get_vacant_jobs()
        existing_jobadvert_ids = self.__data_service.get_vacant_job_id_from_jobadvert()
        logging.info('Zombie is ready to crawl on the web.')

        links = self.__algorithm_service.get_links_by_company_page_url(company_list)

    def __initialize_services(self):
        self.__database = database.DatabaseMSSQL()
        self.__manager = manager.DataManager(self.__database)
        self.__data_service = service.DataService(self.__manager)
        self.__gecko_driver_provider = provider.GeckoDriverProvider()
        self.__algorithm_provider = provider.SearchAlgorithmProvider(self.__data_service)
        self.__algorithm_service = service.AlgorithmService(self.__algorithm_provider, self.__gecko_driver_provider)
