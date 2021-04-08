import logging

import DataLayer.models as models
import DataLayer.providers as providers


class AlgorithmService:
    """
    Handles the communication with the available URLs for the raw data.
    """
    search_algo_provider: providers.SearchAlgorithmProvider
    gecko_driver_provider: providers.GeckoDriverProvider
    raw_data = []

    def __init__(self,
                 algorithm_provider: providers.SearchAlgorithmProvider,
                 gecko_provider: providers.GeckoDriverProvider):
        self.search_algo_provider = algorithm_provider
        self.gecko_driver_provider = gecko_provider

    def find_jobadvert_match_from_raw_source_data(self, raw_data: [str, str, int]) -> models.JobAdvert:
        """
        Algorithm to find jobadvert information, specified by the object.
        Args:
            raw_data: An array of data, contains; 0 is the raw html, 1 is the page url, 2 is the company id.
        Returns:
            JobAdvertModel: the model with compiled data, from the page url.
        """
        self.search_algo_provider.set_page_source(raw_data[0])

        logging.info(f'Attempts to gather job advert data for: {raw_data[1]}')

        if self.search_algo_provider is not None:
            jobadvert = models.JobAdvert(
                0,
                self.search_algo_provider.find_title(),
                self.search_algo_provider.find_email(),
                self.search_algo_provider.find_phone_number(),
                self.search_algo_provider.find_description(),
                self.search_algo_provider.find_location(),
                self.search_algo_provider.find_registration_date(),
                self.search_algo_provider.find_deadline_date(),
                raw_data[1],
                raw_data[2],
                self.search_algo_provider.find_category(),
                self.search_algo_provider.find_specialization()
            )

            return jobadvert

    def get_raw_jobadvert_source_data(self, source_links: []) -> []:
        """
        Achieves the raw data, from an URL.
        Args:
            source_links: A list of URLs
        Returns:
            list: A dataset of raw html data.
        """
        return self.gecko_driver_provider.get_raw_jobadvert_source_data(source_links)

    def get_links_by_company_page_url(self, companies: []) -> []:
        """
        Acquires a list of vacant jobs with the given job advert url, by a list of companies.
        Args:
            companies: list of company
        Returns:
            list: A datalist of vacant jobs
        """
        return self.gecko_driver_provider.get_links_from_company_page_url(companies)

