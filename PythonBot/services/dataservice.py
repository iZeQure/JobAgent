from access.databaseaccess import DatabaseAccess
from objects.jobadvert import JobAdvert


class DataService:
    data_access = DatabaseAccess
    source_links = []
    existing_source_links = []

    def __init__(self):
        self.data_access = DatabaseAccess()

    def initialize_crawler(self) -> bool:
        try:
            self.source_links = self.data_access.get_source_links()
            self.existing_source_links = self.data_access.get_existing_jobadvert_source_links()

            return True
        except ValueError:
            return False

    def save_dataset(self, dataset: JobAdvert):
        try:
            self.data_access.save_dataset(dataset)
        except ValueError:
            return
