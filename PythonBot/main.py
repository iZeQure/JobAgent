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

    jobadvert_data_list = []
    algorithm = AlgorithmService(service)

    print("Acquiring Source Data..")
    raw_data = algorithm.get_raw_data(source_links=service.source_links)
    print("Done.")

    print(f"Compiling [{len(jobadvert_data_list)}] Job Adverts")
    for raw in raw_data:
        jobadvert_data_list.append(algorithm.find_jobadvert_match(raw))
    print("Done.")

    print(f"Saving Job Adverts.")
    for dataset in jobadvert_data_list:
        service.save_dataset(dataset)
    print("Done.")

else:
    print('Initializing Failed.')

print("Shutting Down.")
exit(code=1)
