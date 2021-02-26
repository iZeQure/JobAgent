from access.databaseaccess import DatabaseAccess
from objects.jobadvert import JobAdvert


class DataService:
    """
    Handles the data from and to the database.
    """
    data_access: DatabaseAccess
    source_links: []
    existing_source_links: []
    categories: []
    specializations: []

    def __init__(self):
        self.data_access = DatabaseAccess()
        self.source_links = []
        self.existing_source_links = []
        self.categories = []
        self.specializations = []

    def initialize_crawler(self) -> [bool, str, str]:
        """
        Initializes the Crawler, with needed data from the database.
        @return: True on no errors.
        """
        try:
            information = self.data_access.get_initialization_information()

            if information is not None:
                values = [str, str]

                for arg in information:
                    values[0] = arg[0]
                    values[1] = arg[1]
                    break

                return True, values[0], values[1]

            return False

        except ValueError:
            return False

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
