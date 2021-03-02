import logging
import time

from scriptdir.providers.searchalgorithmprovider import SearchAlgorithmProvider
from scriptdir.services.algorithmservice import AlgorithmService
from scriptdir.services.dataservice import DataService

# logging.basicConfig(level=logging.INFO,
#                     filename='zombie.log',
#                     filemode='w',
#                     format='%(asctime)s [%(levelname)s] - %(message)s',
#                     datefmt='%m/%d/%Y %I:%M:%S %p')
logging.basicConfig(level=logging.INFO,
                    format='%(asctime)s [%(levelname)s] - %(message)s',
                    datefmt='%m/%d/%Y %I:%M:%S %p')

"""
Starts Job Agent Crawler
"""

""" Initialize Services """
logging.info("Starting Services..")

data_service = DataService()
algorithm_provider = SearchAlgorithmProvider(data_service=data_service)
algorithm_service = AlgorithmService(algorithm_provider=algorithm_provider)

try:
    logging.info('Initializing Crawler..')
    initialized_information = data_service.initialize_crawler()

    """ Shutdown Crawler, if init failed. """
    if initialized_information[0] is False:
        logging.error('Initializing Failed.')
        logging.error('Initializing Failed.')
    else:
        logging.info(f'{initialized_information[1]} Initialized on v{initialized_information[3]}')
        logging.info(f'Responsibility: {initialized_information[2]}')

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
