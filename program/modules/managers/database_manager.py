from program.modules.database.db import DatabaseMSSQL
from program.modules.managers.manager import Manager
from program.modules.objects.address import Address
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.job_page import JobPage
from program.modules.objects.vacant_job import VacantJob


class DatabaseManager(Manager):
    def __init__(self, database: DatabaseMSSQL):
        super().__init__(database)

    def get_system_information(self, system_name: str) -> []:
        sp_sql = 'EXEC [JA.spGetSystemInformationByName] @systemName=?;'
        system_info = self.get_sql_data(sp_sql, system_name, fetch_mode='ONE')
        return system_info

    def get_algorithm_keywords_by_key_value(self, key_value: str):
        sp_sql = 'EXEC [GetKeysByKeyValue] @key_value=?'
        return self.get_sql_data(sp_sql, key_value, db_scheme='ZombieCrawlerDB')

    def get_filter_keys(self):
        sp_sql = 'EXEC [JA.spGetStaticFilterKeys]'
        filters = [(f[1], f[2]) for f in self.get_sql_data(sp_sql)]
        return filters

    def get_search_words(self):
        sp_sql = 'EXEC [JA.spGetDynamicFilterKeys]'
        filters = [(x[1], x[2], x[3]) for x in self.get_sql_data(sp_sql)]
        return filters

    def get_categories(self):
        sp_sql = 'EXEC [JA.spGetCategories]'
        return self.get_sql_data(sp_sql)

    def get_specializations(self):
        sp_sql = 'EXEC [JA.spGetSpecializations]'
        return self.get_sql_data(sp_sql)

    def get_companies(self):
        sp_sql = 'EXEC [JA.spGetCompanies]'
        output = [Company(row[0], row[1], row[2], row[3], "None") for row in self.get_sql_data(sp_sql)]
        return output

    def get_job_pages(self):
        sp_sql = 'SELECT * FROM [JobPage]'
        output = [JobPage(row[0], row[1], row[2], '') for row in self.get_sql_data(sp_sql)]
        return output

    def get_vacant_jobs(self) -> [VacantJob]:
        sp_sql = 'EXEC [JA.spGetVacantJobs]'
        output = []
        for vacant_job in self.get_sql_data(sp_sql):
            obj = VacantJob(
                vacant_job_id=vacant_job[0],
                url=vacant_job[1],
                company_id=vacant_job[2],
                page_source=''
            )
            output.append(obj)
        return output

    def get_job_adverts(self):
        sql_text = 'SELECT [VacantJobId] FROM [JobAdvert]'
        output = []
        for vacant_job in self.get_sql_data(sql_text):
            output.append(vacant_job[0])
        return output

    def create_job_advert(self, job_advert: JobAdvert):
        sp_sql = "EXEC [JA.spCreateJobAdvert] @vacantJobId=?,@categoryId=?,@specializationId=?,@jobAdvertTitle=?,@jobAdvertSummary=?,@jobAdvertDescription=?,@jobAdvertEmail=?,@jobAdvertPhoneNr=?,@jobAdvertRegistrationDateTime=?,@jobAdvertApplicationDeadlineDateTime=?;"
        params = (
            job_advert.id,
            job_advert.get_category_id,
            job_advert.get_specialization_id,
            job_advert.get_title,
            job_advert.get_summary,
            job_advert.get_,
            job_advert.get_email,
            job_advert.get_phone_number,
            job_advert.get_registration_datetime,
            job_advert.get_application_deadline_datetime
        )
        self.save_data(sp_sql, params, auto_commit=True)
        self.__create_address(job_advert.get_address)

    def __create_address(self, address: Address):
        sp_sql = "EXEC [JA.spCreateAddress]@jobAdvertVacantJobId=?,@streetAddress=?,@city=?, @country=?, @postalCode=?;"
        params = (
            address.get_job_advert_vacant_job_id,
            address.get_street_address,
            address.get_city,
            address.get_country,
            address.get_postal_code
        )
        self.save_data(sp_sql, params, auto_commit=True)

    def create_vacant_job(self, vacant_job: VacantJob):
        sp_sql = """EXEC [JA.spCreateVacantJob]
                @companyId=?,
                @url=?;
                """
        params = (
            vacant_job.get_company_id,
            vacant_job.get_url
        )
        self.save_data(sp_sql, params, auto_commit=True)

    def update_job_advert(self, job_advert: JobAdvert):
        sp_sql = """EXEC [JA.spUpdateJobAdvert]
                @vacantJobId=?, 
                @categoryId=?, 
                @specializationId=?,
                @jobAdvertTitle=?, 
                @jobAdvertSummary=?,
                @jobAdvertDescription=?, 
                @jobAdvertEmail=?, 
                @jobAdvertPhoneNr=?,
                @jobAdvertRegistrationDateTime=?, 
                @jobAdvertApplicationDeadlineDateTime=?;
                """
        params = (
            job_advert.get_entity_id,
            job_advert.get_category_id,
            job_advert.get_specialization_id,
            job_advert.get_title,
            job_advert.get_summary,
            job_advert.get_,
            job_advert.get_email,
            job_advert.get_phone_number,
            job_advert.get_registration_datetime,
            job_advert.get_application_deadline_datetime
        )
        self.save_data(sp_sql, params, auto_commit=True)

    def update_vacant_job(self, vacant_job: VacantJob):
        sp_sql = """EXEC [JA.spUpdateVacantJob]
        @vacantJobId=?,
        @companyId=?,
        @url=?
        """
        params = (vacant_job.get_entity_id, vacant_job.get_company_id, vacant_job.get_url)
        self.save_data(sp_sql, params)
