from program.modules.managers.database_manager import DatabaseManager
from program.modules.objects.job_advert import JobAdvert


class DataService(object):
    __manager: DatabaseManager

    def __init__(self, manager: DatabaseManager):
        self.__manager = manager

    def get_crawler_information(self):
        try:
            information = self.__manager.get_crawler_information()

            if information is not None:
                return True, information[0], information[1], information[2]

            return False
        except ValueError:
            return False

    def get_algorithm_keywords_by_key(self, key_value: str):
        return self.__manager.get_algorithm_keywords_by_key_value(key_value)

    def get_categories(self):
        return self.__manager.get_categories()

    def get_specializations(self):
        return self.__manager.get_specializations()

    def get_companies(self):
        return self.__manager.get_companies()

    def get_vacant_jobs(self):
        return self.__manager.get_vacant_jobs()

    def get_existing_job_adverts(self):
        return self.__manager.get_existing_job_adverts()

    def create_job_advert(self, jobadvert: JobAdvert):
        self.__manager.create_job_advert(jobadvert)

    def update_job_advert(self, jobadvert):
        self.__manager.update_job_advert(jobadvert)
