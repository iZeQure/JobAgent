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

    def initialize_crawler(self) -> bool:
        """
        Initializes the Crawler, with needed data from the database.
        @return: True on no errors.
        """
        try:
            self.source_links = self.data_access.get_source_links()
            self.existing_source_links = self.data_access.get_existing_jobadvert_source_links()

            self.categories = self.data_access.get_categories()
            self.specializations = self.data_access.get_category_specializations()

            return True
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
