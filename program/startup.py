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
        initialized_information = self.__data_manager.get_crawler_information()

        # Validate the initialization progress.
        if initialized_information is False:
            raise ValueError('Initializing Failed.')
        else:
            # Data Crawling
            # 1. Compiles a list of vacant job urls per company job page url.
            # 2. Compiles the list of vacant jobs into jobadvert.

            info_message = str
            version_message = str
            responsibility_message = str

            for val in initialized_information:
                info_message = val[0]
                version_message = val[2]
                responsibility_message = val[1]

            log.info(f'{info_message} started running as {responsibility_message} on {version_message}')

            # Start Algorithms.
            log.info('Starting Vacant Job Search Algorithm..')
            self.__vacant_job_search_algorithm_provider.run_algorithm()
            log.info('Finished Algorithm!')

            # log.info('Starting Job Advert Search Algorithm..')
            # self.__job_advert_search_algorithm_provider.run_algorithm()
            # log.info('Finished Algorithm!')

    def build_service_collection(self):
        # Build service collection.
        log.info('Building service collection..')

        database = DatabaseMSSQL(self.__app_config)
        web_data_provider = WebDataProvider(self.__app_config)

        self.__data_manager = DatabaseManager(database)
        self.__job_advert_search_algorithm_provider = JobAdvertSearchAlgorithmProvider(self.__data_manager, web_data_provider)
        self.__vacant_job_search_algorithm_provider = VacantJobSearchAlgorithmProvider(self.__data_manager, web_data_provider)
