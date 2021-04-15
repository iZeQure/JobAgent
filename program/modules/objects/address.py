class Address:
    __job_advert_vacant_job_id: int
    __street_address: str
    __city: str
    __country: str
    __postal_code: str

    def __init__(self,
                 job_advert_vacant_job_id: int,
                 street_address: str,
                 city: str,
                 country: str,
                 postal_code: str):
        self.__job_advert_vacant_job_id = job_advert_vacant_job_id
        self.__street_address = street_address
        self.__city = city
        self.__country = country
        self.__postal_code = postal_code

    @property
    def job_advert_vacant_job_id(self):
        return self.__job_advert_vacant_job_id

    @property
    def street_address(self):
        return self.__street_address

    @property
    def city(self):
        return self.__city

    @property
    def country(self):
        return self.__country

    @property
    def postal_code(self):
        return self.__postal_code
