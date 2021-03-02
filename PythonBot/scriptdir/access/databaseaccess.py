import pyodbc

from scriptdir.objects.jobadvert import JobAdvert


class DatabaseAccess:
    """
    Handles the access to the database.
    """
    db = pyodbc
    cursor = pyodbc.Cursor

    def connect(self, auto_commit: bool = False, database: str = 'JobAgentDB'):
        """
        Initializes a new connection to the database.
        @param database: Argument specifies the scheme to achieve information from, default is main db.
        @param auto_commit: Optional, default is False.
        """
        conn = self.db.connect('Driver={SQL Server Native Client 11.0};'
                               f'Server=172.17.0.62\\SKPMSSQLSERVER,1433;'
                               f'Database={database};'
                               f'UID=job_agent;'
                               f'PWD=4Ndet0wn;', autocommit=auto_commit)
        self.cursor = conn.cursor()

    def get_initialization_information(self) -> []:
        self.connect(database="ZombieCrawlerDB")

        sql = """
        SELECT 
            [Name], [Description], 
            CONCAT([VersionControl].[Major],'.',[VersionControl].[Minor],'.',[VersionControl].[Patch]) AS VERSION  
        FROM [Crawler] 
        INNER JOIN [VersionControl]
            ON [Crawler].[Id] = [VersionControl].[CrawlerId]
        WHERE 
            [Name] LIKE '%Zombie%';
        """

        return self.cursor.execute(sql)

    def get_keys_by_value(self, key_value: str) -> []:
        self.connect(database='ZombieCrawlerDB')
        sp_sql = f"EXEC [GetKeysByKeyValue] @key_value=?"
        return self.cursor.execute(sp_sql, key_value)

    def get_source_links(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [CompanyId], [Link] FROM [SourceLink]')

    def get_existing_jobadvert_source_links(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [SourceURL] FROM [JobAdvert]')

    def get_categories(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [Id], [Name] FROM [Category] WHERE [Name] IS NOT NULL')

    def get_category_specializations(self) -> []:
        self.connect()
        return self.cursor.execute('SELECT [Id], [Name] FROM [Specialization] WHERE [Name] IS NOT NULL')

    def save_dataset(self, dataset: JobAdvert):
        self.connect(True, database='ZombieCrawlerDB')
        sp_sql = """\
        EXEC [InitializeJobAdvert] @title=?, @email=?, @phone=?, @desc=?, @location=?, @registered=?, 
        @deadline=?, @source_url=?, @company_id=?, @category_id=?, @specialization_id=?
        """
        params = (dataset.title, dataset.email, dataset.phone_number, dataset.description,
                  dataset.location, dataset.registered_date, dataset.deadline_date,
                  dataset.source_url, dataset.company_id, dataset.category_id,
                  dataset.category_specialization_id)

        self.cursor.execute(sp_sql, params)
