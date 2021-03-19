import logging
import time

from WebCrawler.constants.constant import Constant
from WebCrawler.providers.search_algorithm_provider import SearchAlgorithmProvider
from WebCrawler.services.algorithm_service import AlgorithmService
from WebCrawler.services.data_service import DataService

logging.basicConfig(level=logging.INFO,
                    format='%(asctime)s [%(levelname)s] - %(message)s',
                    datefmt='%m/%d/%Y %I:%M:%S %p')

"""
Starts Job Agent Crawler
"""

""" Initialize Services """
logging.info("Starting Services..")

constant = Constant()
data_service = DataService()
algorithm_provider = SearchAlgorithmProvider(data_service=data_service, constant=constant)
algorithm_service = AlgorithmService(algorithm_provider=algorithm_provider)

try:
    logging.info('Initializing Crawler..')
    initialized_information = data_service.initialize_crawler()

    """ Shutdown Crawler, if init failed. """
    if initialized_information[0] is False:
        logging.error('Initializing Failed.')
    else:
        logging.info(f'{initialized_information[1]} Initialized on v{initialized_information[3]}'
                     f'\nResponsibility: {initialized_information[2]}')

        logging.info("Acquiring Source Data..")
        raw_data = algorithm_service.get_raw_data(source_links=data_service.get_source_links())

        jobadvert_data_list = []
        logging.info(f"Compiling Job Adverts..")
        for raw in raw_data:
            jobadvert_data_list.append(algorithm_service.find_jobadvert_match(raw))

        logging.info(f"Saving <{len(jobadvert_data_list)}> compiled job advert(s).")
        for dataset in jobadvert_data_list:
            data_service.save_dataset(dataset)

        logging.warning('Exiting in 10 seconds.')
        time.sleep(10)
except ValueError:
    logging.exception("Error: 40 - Uncaught Exception.")
