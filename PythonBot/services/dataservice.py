from access.databaseaccess import DatabaseAccess


class DataService:
    data_access = DatabaseAccess
    source_links = []
    existing_source_links = []

    def __init__(self):
        self.data_access = DatabaseAccess()

    def initialize_crawler(self) -> bool:
        self.source_links = self.data_access.get_source_links()
        self.existing_source_links = self.data_access.get_existing_jobadvert_source_links()

        return True
