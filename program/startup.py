import logging as log
from program.modules.providers.web_data_provider import WebDataProvider
from program.modules.providers.vacant_job_search_algorithm_provider import VacantJobSearchAlgorithmProvider
from program.modules.providers.job_advert_search_algorithm_provider import JobAdvertSearchAlgorithmProvider
from program.modules.managers.database_manager import DatabaseManager
from program.modules.database.db import DatabaseMSSQL

# TODO:
# Get a list of Job Pages.
# Find all Vacant Jobs per Job Page.
# Save found Vacant Job as a Job Advert.


class Startup(object):
    __app_config: object
    __data_manager: DatabaseManager
    __job_advert_search_algorithm_provider: JobAdvertSearchAlgorithmProvider
    __vacant_job_search_algorithm_provider: VacantJobSearchAlgorithmProvider

    def __init__(self, app_config: object):
        self.__app_config = app_config

    def initialize_zombie(self):
        """
        Initialize the zombie, to crawl on the web.
        """
        # Crawler Information
        # Gathers the information from the database, to display information on execution.

        log.info('Initializing Crawler..')
        system_info = self.__data_manager.get_system_information(self.__app_config['SystemName'])

        # Validate the initialization progress.
        if len(system_info) == int(0):
            raise ValueError('Initializing Failed. Information was not loaded correctly or information was empty.')
        else:
            # Data Crawling
            # 1. Compiles a list of vacant job urls per company job page url.
            # 2. Compiles the list of vacant jobs into jobadvert.

            log.info(f'Initialized {system_info[1]} on latest: {system_info[0]}:{system_info[2]}:{system_info[4]}')

            # Start Algorithms.
            log.info('Starting Vacant Job Search Algorithm..')
            self.__vacant_job_search_algorithm_provider.run_algorithm()
            log.info('Finished Algorithm!')

            log.info('Starting Job Advert Search Algorithm..')
            self.__job_advert_search_algorithm_provider.run_algorithm()
            log.info('Finished Algorithm!')

    def build_service_collection(self):
        # Build service collection.
        log.info('Building service collection..')

        database = DatabaseMSSQL(self.__app_config)
        web_data_provider = WebDataProvider(self.__app_config)

        self.__data_manager = DatabaseManager(database)
        self.__job_advert_search_algorithm_provider = JobAdvertSearchAlgorithmProvider(self.__data_manager, web_data_provider)
        self.__vacant_job_search_algorithm_provider = VacantJobSearchAlgorithmProvider(self.__data_manager, web_data_provider)
