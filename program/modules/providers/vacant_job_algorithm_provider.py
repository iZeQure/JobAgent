from program.modules.services.data_service import DataService
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, data_service: DataService):
        super().__init__(data_service)