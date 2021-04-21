import logging as log
from program.modules.services.data_service import DataService
from program.modules.providers.search_algorithm_provider import SearchAlgorithmProvider
from program.modules.objects.vacant_job import VacantJob
from program.modules.objects.company import Company


class VacantJobSearchAlgorithmProvider(SearchAlgorithmProvider):
    def __init__(self, data_service: DataService):
        super().__init__(data_service)

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

            print(f'Testing this link for information: {link}')

            if link.text:
                log.info(f'Gathered [{link["href"]}]')
                useful_links.append(link['href'])

        return useful_links

    def get_data(self, company: object) -> []:
        if company is None:
            raise ValueError('Parameter was type of None.')
        else:
            if isinstance(company, Company):
                log.info(f'Processing gatherer with [{company.id}] for [{company.job_page_url}].')

                self.set_page_source(company.html_page_source)

                vacant_job_list = []

                for item in self.__find_vacant_job_links():
                    vacant_job = VacantJob(
                        vacant_job_id=0,
                        link=item[0],
                        company_id=company.id,
                        html_page_source=company.html_page_source
                    )

                    vacant_job_list.append(vacant_job)

                return vacant_job_list
            else:
                raise TypeError('Given type was not of type Company.')
