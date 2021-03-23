import logging
import time

from WebCrawler.providers import GeckoDriverProvider, SearchAlgorithmProvider
from WebCrawler.services.algorithm_service import AlgorithmService
from WebCrawler.services.data_service import DataService


class Startup:
    __data_service: DataService
    __gecko_driver_provider: GeckoDriverProvider
    __algorithm_provider: SearchAlgorithmProvider
    __algorithm_service: AlgorithmService

    def __init__(self):
        logging.basicConfig(level=logging.INFO,
                            format='%(asctime)s [%(levelname)s] - %(message)s',
                            datefmt='%m/%d/%Y %I:%M:%S %p')

    def start_crawler(self):
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

                self.__compile_job_adverts_by_found_source_links()

        except ValueError:
            logging.exception("Error: 40 - Uncaught Exception.")

    def __compile_job_adverts_by_found_source_links(self) -> None:
        source_links = self.__data_service.get_source_links()
        try:
            if len(source_links) != 0:
                logging.info("Acquiring Source Data..")
                raw_data = self.__algorithm_service.get_raw_data(source_links=source_links)

                if len(raw_data) != 0:
                    jobadvert_data_list = []
                    logging.info(f"Compiling Job Adverts..")
                    for raw in raw_data:
                        jobadvert_data_list.append(self.__algorithm_service.find_jobadvert_match(raw))

                    if len(jobadvert_data_list) != 0:
                        logging.info(f"Saving <{len(jobadvert_data_list)}> compiled job advert(s).")
                        for dataset in jobadvert_data_list:
                            self.__data_service.save_dataset(dataset)
                    else:
                        logging.info('No job adverts were compiled.')
                else:
                    logging.info('No Data was found.')
            else:
                logging.info('No Source Links were found.')
        finally:
            logging.warning('Exiting in 10 seconds.')
            time.sleep(10)

    def __initialize_services(self):
        self.__data_service = DataService()

        self.__gecko_driver_provider = GeckoDriverProvider()

        self.__algorithm_provider = SearchAlgorithmProvider(
            data_service=self.__data_service)

        self.__algorithm_service = AlgorithmService(
            algorithm_provider=self.__algorithm_provider,
            gecko_provider=self.__gecko_driver_provider)
