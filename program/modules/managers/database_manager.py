from program.modules.database.db import DatabaseMSSQL
from program.modules.managers.manager import Manager
from program.modules.objects.address import Address
from program.modules.objects.job_advert import JobAdvert
from program.modules.objects.vacant_job import VacantJob


class DatabaseManager(Manager):
    def __init__(self, database: DatabaseMSSQL):
        super().__init__(database)

    def get_crawler_information(self):
        sp_sql = 'EXEC [GetInitializationInformation]'
        return self.get_data(sp_sql, db_scheme='ZombieCrawlerDB')

    def get_algorithm_keywords_by_key_value(self, key_value: str):
        sp_sql = 'EXEC [GetKeysByKeyValue] @key_value=?'
        return self.get_data(sp_sql, key_value, db_scheme='ZombieCrawlerDB')

    def get_categories(self):
        sp_sql = 'EXEC [JA.spGetCategories]'
        return self.get_data(sp_sql)

    def get_specializations(self):
        sp_sql = 'EXEC [JA.spGetSpecializations]'
        return self.get_data(sp_sql)

    def get_companies(self):
        sp_sql = 'EXEC [JA.spGetCompanies]'
        return self.get_data(sp_sql)

    def get_vacant_jobs(self) -> [VacantJob]:
        sp_sql = 'EXEC [JA.spGetVacantJobs]'
        output = []
        for vacant_job in self.get_data(sp_sql):
            obj = VacantJob(
                vacant_job_id=vacant_job[0],
                link=vacant_job[1],
                company_id=vacant_job[2],
                html_page_source=''
            )
            output.append(obj)
        return output

    def get_existing_job_adverts(self):
        sql_text = 'SELECT [VacantJobId] FROM [JobAdvert]'
        output = []
        for vacant_job in self.get_data(sql_text):
            output.append(vacant_job[0])
        return output

    def create_job_advert(self, job_advert: JobAdvert):
        sp_sql = """EXEC [JA.spCreateJobAdvert]
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
            job_advert.id,
            job_advert.category_id,
            job_advert.specialization_id,
            job_advert.title,
            job_advert.summary,
            job_advert.description,
            job_advert.email,
            job_advert.phone_number,
            job_advert.registration_datetime,
            job_advert.application_deadline_datetime
        )
        self.save_data(sp_sql, params, auto_commit=True)

    def create_address(self, address: Address):
        sp_sql = """EXEC [JA.spCreateAddress]
                @jobAdvertVacantJobId=?,
                @streetAddress=?,
                @city=?,
                @postalCode=?;
                """
        params = (
            address.job_advert_vacant_job_id,
            address.street_address,
            address.city,
            address.country,
            address.postal_code
        )
        self.save_data(sp_sql, params, auto_commit=True)

    def create_vacant_job(self, vacant_job: VacantJob):
        sp_sql = """EXEC [JA.spCreateVacantJob]
                @vacantJobLink=?,
                @companyId=?;
                """
        params = (
            vacant_job.link,
            vacant_job.company_id
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
            job_advert.id,
            job_advert.category_id,
            job_advert.specialization_id,
            job_advert.title,
            job_advert.summary,
            job_advert.description,
            job_advert.email,
            job_advert.phone_number,
            job_advert.registration_datetime,
            job_advert.application_deadline_datetime
        )
        self.save_data(sp_sql, params, auto_commit=True)
