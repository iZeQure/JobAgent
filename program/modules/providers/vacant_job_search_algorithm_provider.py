import logging as log
from program.modules.managers.database_manager import DatabaseManager
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.objects.vacant_job import VacantJob
from program.modules.objects.company import Company


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, manager: DatabaseManager):
        super().__init__(manager)

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

    def get_data(self, company: object) -> []:
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
