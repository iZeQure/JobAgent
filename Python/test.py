import requests
from bs4 import BeautifulSoup

page = requests.get("https://dk.grundfos.com/om-os/karriere/jobs/job.html?jobid=46029")

data = page.text
soup = BeautifulSoup(data, 'html.parser')

print(soup.prettify())

