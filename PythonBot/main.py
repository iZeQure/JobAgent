from services.dataservice import DataService
from services.algorithmservice import AlgorithmService
from providers.searchalgorithmprovider import SearchAlgorithmProvider

"""
Starts Job Agent Crawler
"""

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

    print(f'{initialized_information[1]} Initialized on v{initialized_information[3]}\n'
          f'Responsibility: {initialized_information[2]}')

    print("Acquiring Source Data..")
    raw_data = algorithm_service.get_raw_data(source_links=data_service.get_source_links())

    jobadvert_data_list = []
    print(f"Compiling Job Adverts..")
    for raw in raw_data:
        jobadvert_data_list.append(algorithm_service.find_jobadvert_match(raw))

    print(f"Saving <{len(jobadvert_data_list)}> compiled job advert(s).")
    for dataset in jobadvert_data_list:
        data_service.save_dataset(dataset)

    print("Shutting Down.")
    exit(code=1)
except ValueError:
    print("Error: 40 - Uncaught Exception.")
    exit(code=40)
