from services.dataservice import DataService
from services.algorithmservice import AlgorithmService

"""
Initializes Job Agent Crawler v1.0
"""
print('Initializing Crawler..')

service = DataService()

is_initialized = service.initialize_crawler()

if is_initialized:
    print('Crawler Initialized.')

    algorithm = AlgorithmService()

    raw_data = algorithm.get_raw_data(source_links=service.source_links)

    jobadvert = algorithm.find_jobadvert_match(raw_data[0])

    print(jobadvert)

else:
    print('Initializing Failed.')
