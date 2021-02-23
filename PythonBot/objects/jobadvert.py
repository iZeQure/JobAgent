import datetime


class JobAdvert:
    id = int
    title = str
    email = str
    phone_number = str
    description = str
    location = str
    registered_date = datetime
    deadline_date = datetime
    source_url = str
    company_id= int
    category_id = int
    category_specialization_id = int

    def __init__(self, jobadvert_id, title, email, phone_number,
                 description, location, reg_date,
                 deadline_date, source_url, company_id,
                 category_id, category_specialization_id):
        """
        Initializes a new job advert class.
        @param jobadvert_id: an identifier of the object.
        @param title: a title of the job advert.
        @param email: contact information for the job advert.
        @param phone_number: contact information for the job advert.
        @param description: a text containing the description.
        @param location: location of the job advert.
        @param reg_date: when is the job advert registered.
        @param deadline_date: when is the job advert is expired.
        @param source_url: the original job advert link.
        @param company_id: an identifier of the company, owning the job advert.
        @param category_id: identifies the category of the job advert.
        @param category_specialization_id: is zero, if specialization isn't associated with a category.
        """
        self.company_id = company_id
        self.deadline_date = deadline_date
        self.reg_date = reg_date
        self.location = location
        self.description = description
        self.phone_number = phone_number
        self.category_id = category_id
        self.source_url = source_url
        self.category_specialization_id = category_specialization_id
        self.email = email
        self.title = title
        self.id = jobadvert_id

