from re import compile
from program.modules.services.data_service import DataService
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, data_service: DataService):
        super().__init__(data_service)

    def find_vacant_job_links(self) -> []:
        useful_links = []
        for link in self.soup.find_all('a'):
            if link.text:
                useful_links.append(link['href'])

        return
