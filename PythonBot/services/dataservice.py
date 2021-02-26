from access.databaseaccess import DatabaseAccess
from objects.jobadvert import JobAdvert


class DataService:
    """
    Handles the data from and to the database.
    """
    data_access: DatabaseAccess
    categories: []
    specializations: []

    def __init__(self):
        self.data_access = DatabaseAccess()
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

    def save_dataset(self, dataset: JobAdvert):
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
