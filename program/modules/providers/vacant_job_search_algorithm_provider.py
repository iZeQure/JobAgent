import logging as log
from program.modules.managers.database_manager import DatabaseManager
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.objects.vacant_job import VacantJob
from program.modules.objects.company import Company
from program.modules.providers.web_data_provider import WebDataProvider


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, manager: DatabaseManager, web_data: WebDataProvider):
        super().__init__(manager, web_data)

    def run_algorithm(self):
        # Flow
        # Get a valid list of companies, with their associated job page(s).
        # Get a list of vacant job(s) per job page(s).
        # Store the found vacant job(s) in the database.

        try:
            company_list = self.manager.get_companies()
            if company_list is None:
                raise Exception('Invalid data list, no companies found.')
            else:
                if len(company_list) == int(0):
                    raise ValueError('No data was found in the company data list.')
                else:
                    pass

        except ValueError as er:
            log.warning(er)
        except Exception as ex:
            log.exception(ex)

    def __find_vacant_job_links(self) -> []:
        # job_iframe = self.soup.find_all(attrs={"aria-label": "Job widget"})
        #
        # iframe = str
        # if job_iframe is not None:
        #     for job in job_iframe:
        #         iframe = job['srcdoc']
        # print(f'A found iframe: {iframe}')

        useful_links = []
        for link in self.soup.find_all('a'):
            if link.text:
                useful_links.append(link['href'])

        return useful_links

    def __get_data(self, company: object) -> []:
        if company is None:
            raise ValueError('Parameter was type of None.')
        else:
            if isinstance(company, Company):
                log.info(f'Processing algorithm with [{company.id}] for [{company.job_page_url}].')

                self.set_page_source(company.html_page_source)

                vacant_job_list = []

                for item in self.__find_vacant_job_links():
                    log.info(f'Gathered [{item}] for {company.id}')

                    vacant_job = VacantJob(
                        vacant_job_id=0,
                        link=item[0],
                        company_id=company.id,
                        html_page_source=company.html_page_source
                    )

                    vacant_job_list.append(vacant_job)

                log.info(f'Found [{len(vacant_job_list)}] for company [{company.id}]')
                return vacant_job_list
            else:
                raise TypeError('Given type was not of type Company.')

    def __get_vacant_job_list_from_company(self, company: Company) -> []:
        if company is None:
            raise Exception('Expected parameter Comapny, but got None.')
        else:
            data_list = self.__get_data(company)

            return data_list
