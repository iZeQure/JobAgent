import logging

import DataLayer.models as models
import DataLayer.providers as providers


class AlgorithmService:
    """
    Handles the communication with the available URLs for the raw data.
    """
    __search_algorithm_provider: providers.SearchAlgorithmProvider
    __page_source_provider: providers.PageSourceProvider

    def __init__(self,
                 algorithm_provider: providers.SearchAlgorithmProvider,
                 gecko_provider: providers.PageSourceProvider):
        self.__search_algorithm_provider = algorithm_provider
        self.__page_source_provider = gecko_provider

    def compile_jobadvert_from_vacant_job_data_list(self, vacant_job):
        """
        Algorithm to find jobadvert information, specified by the object.
        Args:
            vacant_job: An array of data, contains; 0 is the raw html, 1 is the page url, 2 is the company id.
        Returns:
            JobAdvertModel: the model with compiled data, from the page url.
        """
        self.__search_algorithm_provider.set_page_source(vacant_job[3])

        logging.info(f'Attempts to gather job advert data for: {vacant_job[1]}')

        if self.__search_algorithm_provider is not None:
            jobadvert = models.JobAdvert(
                vacant_job_id=vacant_job[0],
                category_id=self.__search_algorithm_provider.find_category(),
                specialization_id=self.__search_algorithm_provider.find_specialization(),
                title=self.__search_algorithm_provider.find_title(),
                summary='None',
                description=self.__search_algorithm_provider.find_description(),
                email=self.__search_algorithm_provider.find_email(),
                phone_number=self.__search_algorithm_provider.find_phone_number(),
                registration_datetime=self.__search_algorithm_provider.find_registration_date(),
                application_deadline_datetime=self.__search_algorithm_provider.find_deadline_date(),
                address=models.Address(
                    job_advert_vacant_job_id=vacant_job[0],
                    street_address=self.__search_algorithm_provider.find_location(),
                    city='None',
                    country='None',
                    postal_code='None'
                )
            )

            return jobadvert

    def get_html_page_source_data_list(self, data_list: []):
        """
        Achieves the html page source, from an URL.
        Args:
            data_list: A list of data, containing the URL's.
        Returns:
            list: A list of data with the html page source.
        """
        return self.__page_source_provider.get_html_page_source_data_list(data_list)

    def get_links_by_company_page_url(self, companies: []):
        """
        Acquires a list of vacant jobs with the given job advert url, by a list of companies.
        Args:
            companies: list of company
        Returns:
            list: A datalist of vacant jobs
        """
        return self.__page_source_provider.get_links_from_company_page_url(companies)

