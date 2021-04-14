import logging

import WebCrawler.DataLayer.models as models
import WebCrawler.DataLayer.providers as providers


class AlgorithmService:
    """
    Handles the communication with the available URLs for the raw data.
    """
    __jobadvert_search_algorithm: providers.JobAdvertSearchAlgorithmProvider
    __vacantjob_search_algorithm: providers.VacantJobSearchAlgorithmProvider
    __page_source_provider: providers.PageSourceProvider

    def __init__(self,
                 jobadvert_search_algorithm: providers.JobAdvertSearchAlgorithmProvider,
                 vacantjob_search_algorithm: providers.VacantJobSearchAlgorithmProvider,
                 gecko_provider: providers.PageSourceProvider):

        self.__jobadvert_search_algorithm = jobadvert_search_algorithm
        self.__vacantjob_search_algorithm = vacantjob_search_algorithm
        self.__page_source_provider = gecko_provider

    def compile_jobadvert_data_object(self, vacant_job) -> models.JobAdvert:
        """
        Algorithm to find jobadvert information, specified by the object.
        Args:
            vacant_job: An array of data, contains; 0 is the raw html, 1 is the page url, 2 is the company id.
        Returns:
            JobAdvertModel: the model with compiled data, from the page url.
        """
        self.__jobadvert_search_algorithm.set_page_source(vacant_job[3])

        logging.info(f'Attempts to gather job advert data for: {vacant_job[1]}')

        if self.__jobadvert_search_algorithm is not None:
            jobadvert = models.JobAdvert(
                vacant_job_id=vacant_job[0],
                category_id=self.__jobadvert_search_algorithm.find_category(),
                specialization_id=self.__jobadvert_search_algorithm.find_specialization(),
                title=self.__jobadvert_search_algorithm.find_title(),
                summary='None',
                description=self.__jobadvert_search_algorithm.find_description(),
                email=self.__jobadvert_search_algorithm.find_email(),
                phone_number=self.__jobadvert_search_algorithm.find_phone_number(),
                registration_datetime=self.__jobadvert_search_algorithm.find_registration_date(),
                application_deadline_datetime=self.__jobadvert_search_algorithm.find_deadline_date(),
                address=models.Address(
                    job_advert_vacant_job_id=vacant_job[0],
                    street_address=self.__jobadvert_search_algorithm.find_location(),
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
