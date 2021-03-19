from WebCrawler.data_access.sql_access import SqlAccess
from WebCrawler.models.job_advert_model import JobAdvertModel


class DataService:
    """
    Handles the data from and to the database.
    """
    data_access: SqlAccess
    categories: []
    specializations: []

    def __init__(self):
        self.data_access = SqlAccess()
        self.categories = self.data_access.get_categories()
        self.specializations = self.data_access.get_category_specializations()

    def initialize_crawler(self) -> []:
        """
        Initializes the Crawler, with needed data from the database.
        @return: True on no errors.
        """
        try:
            information = self.data_access.get_initialization_information()

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

    def get_keys_by_value(self, key_value: str) -> []:
        return self.data_access.get_keys_by_value(key_value)

    def save_dataset(self, dataset: JobAdvertModel):
        """
        Saves a Job Advert to the database.
        @param dataset: An object containing the jobadvert.
        @return: Nothing.
        """
        try:
            self.data_access.save_dataset(dataset)
        except ValueError:
            return

    def get_source_links(self) -> []:
        return self.data_access.get_source_links()

    def get_categories(self) -> []:
        return self.data_access.get_categories()

    def get_specializations(self) -> []:
        return self.data_access.get_category_specializations()
