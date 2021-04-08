class Address:
    __jobAdvertVacantJobId = int
    __streetAddress = str
    __city = str
    __country = str
    __postalCode = str

    def __init__(self, job_advert_vacant_job_id: int, street_address: str, city: str, country: str, postal_code: str):
        """
        Creates an object of Address.
        Args:
            job_advert_vacant_job_id: Unique object id, for the jobadvert object entity.
            street_address: The address of the job advert post.
            city: The city of the job advert post.
            country: The country of the job advert post.
            postal_code: The postal code for the city.
        """
        self.__jobAdvertVacantJobId = job_advert_vacant_job_id
        self.__streetAddress = street_address
        self.__city = city
        self.__country = country
        self.__postalCode = postal_code

    @property
    def id(self) -> int:
        return self.__jobAdvertVacantJobId

    @property
    def street_address(self) -> str:
        return self.__streetAddress

    @property
    def city(self) -> str:
        return self.__city

    @property
    def country(self) -> str:
        return self.__country

    @property
    def postal_code(self) -> str:
        return self.__postalCode
