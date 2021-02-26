from services.dataservice import DataService
from services.algorithmservice import AlgorithmService
from providers.searchalgorithmprovider import SearchAlgorithmProvider

"""
Initializes Job Agent Crawler v1.0
"""

jobadvert_data_list: []

""" Initialize Services """
print("Starting Services..")
data_service = DataService()
algorithm_provider = SearchAlgorithmProvider(data_service=data_service)
algorithm_service = AlgorithmService(algorithm_provider=algorithm_provider)

try:
    print('Initializing Crawler..')
    initialized_information = data_service.initialize_crawler()

    """ Shutdown Crawler, if init failed. """
    if initialized_information[0] is False:
        print('Initializing Failed.')
        exit(501)

    print(f'{initialized_information[1]} Initialized -> {initialized_information[2]}')

    print("Acquiring Source Data..")
    raw_data = algorithm_service.get_raw_data(source_links=data_service.get_source_links())
    print("Done.")

    print(f"Compiling [{len(jobadvert_data_list) + 1}] Job Adverts")
    jobadvert_data_list = []
    for raw in raw_data:
        jobadvert_data_list.append(algorithm_service.find_jobadvert_match(raw))
    print("Done.")

    print(f"Saving Job Adverts.")
    for dataset in jobadvert_data_list:
        data_service.save_dataset(dataset)
    print("Done.")

    print("Shutting Down.")
    exit(code=1)
except ValueError:
    print("Error: 40 - Uncaught Exception.")
    exit(code=40)
