import logging as log
from program.modules.providers.job_advert_search_algorithm_provider import JobAdvertSearchAlgorithmProvider
from program.modules.providers.vacant_job_search_algorithm_provider import VacantJobSearchAlgorithmProvider
from program.modules.providers.web_data_provider import WebDataProvider
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob
from program.modules.objects.company import Company
from program.modules.objects.address import Address


class AlgorithmService(object):
    """
        Handles the communication with the available URLs for the raw data.
    """
    __job_advert_search_algorithm: JobAdvertSearchAlgorithmProvider
    __vacant_job_search_algorithm: VacantJobSearchAlgorithmProvider
    __web_data_provider: WebDataProvider

    def __init__(self,
                 job_advert_search_algorithm_provider: JobAdvertSearchAlgorithmProvider,
                 vacant_job_search_algorithm_provider: VacantJobSearchAlgorithmProvider,
                 web_data_provider: WebDataProvider):
        self.__job_advert_search_algorithm = job_advert_search_algorithm_provider
        self.__vacant_job_search_algorithm = vacant_job_search_algorithm_provider
        self.__web_data_provider = web_data_provider

    def compile_jobadvert_data_object(self, vacant_job: VacantJob) -> JobAdvert:
        """
        Algorithm to find jobadvert information, specified by the object.
        Args:
            vacant_job: An array of data, contains; 0 is the raw html, 1 is the page url, 2 is the company id.
        Returns:
            JobAdvertModel: the model with compiled data, from the page url.
        """
        self.__job_advert_search_algorithm.set_page_source(vacant_job.html_page_source)

        log.info(f'Attempts to gather job advert data for: {vacant_job.link}')

        if self.__job_advert_search_algorithm is not None:
            jobadvert = JobAdvert(
                vacant_job_id=vacant_job.id,
                category_id=self.__job_advert_search_algorithm.find_category(),
                specialization_id=self.__job_advert_search_algorithm.find_specialization(),
                title=self.__job_advert_search_algorithm.find_title(),
                summary='None',
                description=self.__job_advert_search_algorithm.find_description(),
                email=self.__job_advert_search_algorithm.find_email(),
                phone_number=self.__job_advert_search_algorithm.find_phone_number(),
                registered_date_time=self.__job_advert_search_algorithm.find_registration_date(),
                application_deadline_date_time=self.__job_advert_search_algorithm.find_deadline_date(),
                address=Address(
                    job_advert_vacant_job_id=vacant_job.id,
                    street_address=self.__job_advert_search_algorithm.find_location(),
                    city='None',
                    country='None',
                    postal_code='None'
                )
            )

            return jobadvert

    def compile_vacant_job_data_list(self, company: Company) -> []:
        self.__vacant_job_search_algorithm.set_page_source(company.html_page_source)

        log.info(f'Attempting to gather vacant jobs from {company.job_page_url}')
        
        vacant_job_data_list = self.__vacant_job_search_algorithm.find_vacant_job_links()

        return vacant_job_data_list

    def get_html_page_source_data_list_from_vacant_job_list(self, data_list: []):
        """
        Achieves the html page source, from an URL.
        Args:
            data_list: A list of data, containing the URL's.
        Returns:
            list: A list of data with the html page source.
        """
        data = self.__web_data_provider.load_vacant_job_web_data_html(data_list)

        return data

    def load_html_data_by_company(self, company):
        data = self.__web_data_provider.load_html_page_data_by_company(company)

        return data

    def get_links_by_company_page_url(self, companies: []):
        """
        Acquires a list of vacant jobs with the given job advert url, by a list of companies.
        Args:
            companies: list of company
        Returns:
            list: A datalist of vacant jobs
        """
        return self.__web_data_provider.load_html_page_data_by_company(companies)
