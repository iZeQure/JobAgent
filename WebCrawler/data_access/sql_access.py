import pyodbc
import json

from WebCrawler.models.job_advert_model import JobAdvertModel


class SqlAccess:
    """
    Handles the access to the database.
    """
    __db = pyodbc
    __cursor = pyodbc.Cursor
    __db_config = object

    def __init__(self):
        with open('WebCrawler/database.config.json') as config:
            db_config = json.load(config)
            self.__db_config = db_config["DB"]["Local"]

    def connect(self, auto_commit: bool = False, database: str = 'JobAgentDB'):
        """
        Initializes a new connection to the database.
        @param database: Argument specifies the scheme to achieve information from, default is main db.
        @param auto_commit: Optional, default is False.
        """

        conn = object
        try:
            trusted_connection = self.__db_config["Trusted_Connection"]

            if trusted_connection == "Yes":
                conn = self.__db.connect(
                    autocommit=auto_commit,
                    Trusted_Connection=trusted_connection,
                    Driver=f'{self.__db_config["Driver"]}',
                    Server=f'{self.__db_config["Server"]}',
                    Database=f'{database}')
            elif trusted_connection == "No":
                conn = self.__db.connect(
                    autocommit=auto_commit,
                    Trusted_Connection=trusted_connection,
                    Driver=f'{self.__db_config["Driver"]}',
                    Server=f'{self.__db_config["Server"]}',
                    Database=f'{database}',
                    UID=self.__db_config["UID"],
                    PWD=self.__db_config["PWD"])

            self.__cursor = conn.cursor()
        except Exception as ex:
            raise ex

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

        return self.__cursor.execute(sql)

    def get_keys_by_value(self, key_value: str) -> []:
        self.connect(database='ZombieCrawlerDB')
        sp_sql = f"EXEC [GetKeysByKeyValue] @key_value=?"
        return self.__cursor.execute(sp_sql, key_value)

    def get_source_links(self) -> list:
        self.connect()
        return list(self.__cursor.execute('SELECT [CompanyId], [Link] FROM [SourceLink]'))

    def get_existing_jobadvert_source_links(self) -> []:
        self.connect()
        return list(self.__cursor.execute('SELECT [SourceURL] FROM [JobAdvert]'))

    def get_categories(self) -> []:
        self.connect()
        return list(self.__cursor.execute('SELECT [Id], [Name] FROM [Category] WHERE [Name] IS NOT NULL'))

    def get_category_specializations(self) -> []:
        self.connect()
        return list(self.__cursor.execute('SELECT [Id], [Name] FROM [Specialization] WHERE [Name] IS NOT NULL'))

    def save_dataset(self, dataset: JobAdvertModel):
        self.connect(True, database='ZombieCrawlerDB')
        sp_sql = """\
        EXEC [InitializeJobAdvert] @title=?, @email=?, @phone=?, @desc=?, @location=?, @registered=?, 
        @deadline=?, @source_url=?, @company_id=?, @category_id=?, @specialization_id=?
        """
        params = (dataset.title, dataset.email, dataset.phone_number, dataset.description,
                  dataset.location, dataset.registered_date, dataset.deadline_date,
                  dataset.source_url, dataset.company_id, dataset.category_id,
                  dataset.category_specialization_id)

        self.__cursor.execute(sp_sql, params)
