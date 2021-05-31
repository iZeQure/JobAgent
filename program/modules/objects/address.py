from program.modules.handlers.error_handler import ErrorHandler
from program.modules.handlers.property_handler import PropertyHandler
from program.modules.objects.base_job_entity import BaseJobEntity


class Address(BaseJobEntity):
    """
    An object containing basic information about an Address.
    """

    def __init__(self, job_advert_vacant_job_id: int, street_address: str, city: str, country: str, postal_code: str):
        """
        Instantiates an Address entity.
        Args:
            job_advert_vacant_job_id: Defines the unique identify of the associated object.
            street_address: Defines the street address for this object.
            city: Defines the city for this object.
            country: Defines the country for this object.
            postal_code: Defines the postal code for this object.
        """
        super().__init__(job_advert_vacant_job_id)
        self._street_address = street_address
        self._city = city
        self._country = country
        self._postal_code = postal_code

    @property
    def get_street_address(self) -> str:
        """
        Get the street address for this object.
        Returns:
            str: A string representing the street address.

        """
        if PropertyHandler.is_type_of(self._street_address, str):
            return self._street_address

        ErrorHandler.raise_value_error(str, self._street_address, 'Street Address')

    @property
    def get_city(self) -> str:
        """
        Get the city for this object.
        Returns:
            str: A string representing the city.

        """
        if PropertyHandler.is_type_of(self._city, str):
            return self._city

        ErrorHandler.raise_value_error(str, self._city, 'City')

    @property
    def get_country(self) -> str:
        """
        Get the country for this object.
        Returns:
            str: A string representing the country.

        """
        return self._country

    @property
    def get_postal_code(self) -> str:
        """
        Get the postal code for this object.
        Returns:
            str: A string representing the postal code.

        """
        return self._postal_code
