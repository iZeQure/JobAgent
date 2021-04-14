import WebCrawler.DataLayer.access as manager
import WebCrawler.DataLayer.models as models


class DataService:
    """
    Handles the data from and to the database.
    """
    __data_manager: manager.DataManager

    def __init__(self, data_manager):
        self.__data_manager = data_manager

    def get_crawler_information(self):
        """
        Initializes the Crawler, with needed data from the database.
        @return: True on no errors.
        """
        try:
            information = self.__data_manager.get_initialization_information()

            if information is not None:
                values = [str, str, str]

                for arg in information:
                    values[0] = arg[0]
                    values[1] = arg[1]
                    values[2] = arg[2]
                    break

                return True, values[0], values[1], values[2]

            return False

        except ValueError:
            return False

    def get_algorithm_keywords_by_key(self, key_value: str):
        return self.__data_manager.get_algorithm_keywords_by_key_value(key_value)

    def get_categories(self):
        return self.__data_manager.get_categories()

    def get_specializations(self):
        return self.__data_manager.get_specializations()

    def get_companies(self):
        return self.__data_manager.get_companies()

    def get_vacant_jobs(self):
        return self.__data_manager.get_vacant_jobs()

    def get_existing_job_adverts(self):
        return self.__data_manager.get_existing_job_adverts()

    def create_jobadvert(self, jobadvert: models.JobAdvert):
        self.__data_manager.create_job_advert(jobadvert)

    def update_jobadvert(self, jobadvert):
        self.__data_manager.update_jobadvert(jobadvert)
