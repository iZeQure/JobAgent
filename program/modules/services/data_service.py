from program.modules.managers.database_manager import DataManager
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob


class DataService(object):
    __data_manager: DataManager

    def __init__(self, manager: DataManager):
        self.__data_manager = manager

    def get_crawler_information(self):
        try:
            information = self.__data_manager.get_crawler_information()

            if information is not None and information is not False:
                return information

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

    def get_vacant_jobs(self) -> [VacantJob]:
        return self.__data_manager.get_vacant_jobs()

    def get_existing_job_adverts(self):
        return self.__data_manager.get_existing_job_adverts()

    def create_job_advert(self, jobadvert: JobAdvert):
        self.__data_manager.create_job_advert(jobadvert)

    def update_job_advert(self, jobadvert):
        self.__data_manager.update_job_advert(jobadvert)
