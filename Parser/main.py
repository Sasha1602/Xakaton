from bs4 import BeautifulSoup
import requests
import re
import os
from transliterate import translit
from collections import defaultdict

from pymongo import MongoClient
from pymongo import ReturnDocument

client = MongoClient('mongodb://localhost:27017/')
db = client.Xakabd


def my_translit(text):
    res = translit(text, reversed=True).strip().replace(" ", "_").replace("'", "")
    return res


def get_sub_cat(link):
    subcategories = []
    clothes = requests.get(f"{base_link}{link}")
    clothes_soup = BeautifulSoup(clothes.content, "html.parser")

    cat_id = link.split("/")[2]
    cloth_nav = clothes_soup.find("span", attrs={"data-catid": cat_id})
    if len(cloth_nav.parent) == 1:
        return subcategories
    items_list = cloth_nav.parent.contents[1]
    childs = items_list.find_all("a")
    for child in childs:
        name = my_translit(child.string)
        lnk = child.attrs["href"].split("#")[0]
        # print(name, lnk)
        subcategories.append({"name": name, "link": lnk, "rus_name": child.string.strip(), "type": "subcategory"})
    return subcategories


base_link = "https://www.lamoda.ru"
women_home = f"{base_link}/women-home/"
man_home = f"{base_link}/man-home/"
kids_home = f"{base_link}/kids-home/"

bases = {"women": women_home, "man": man_home, "kids": kids_home}

key = list(bases.keys())[0]
base = bases[key]
print(base)
# for base in bases

response = requests.get(base)

page_soup = BeautifulSoup(response.content, "html.parser")
print(page_soup)
items = defaultdict()
items["Одежда"] = defaultdict(list,[("name","cloth")])
#items["Обувь"] = defaultdict(list,[("name","shooes")])


nav = page_soup.find("div", {"class" : "_root_v5ijp_2"})
print(nav)



# Working parsing Categories
for item in items:
    db_categories = db[f"{key}_{items[item]['name']}"]
    categories = []
    element = nav.find_all("a", string=re.compile(f".*{item}"))
    cloth_link = element[0].attrs["href"]

    clothes = requests.get(f"{base_link}{cloth_link}")
    clothes_soup = BeautifulSoup(clothes.content, "html.parser")


    cloth_nav = clothes_soup.find("x-product-card__pic-img", attrs={"data-loaded":"1"})

    items_list = cloth_nav.parent.contents[1]
    for nav_el in items_list:
        cat_link = nav_el.contents[0].find("a")
        name = my_translit(cat_link.string)
        lnk = cat_link.attrs["href"].split("#")[0]
        category = {"name": name, "link": lnk, "rus_name": cat_link.string.strip(), "type": "category"}
        category_db = db_categories.find_one_and_update({"link": lnk},
            {"$set": category},
            upsert=True,
            return_document=ReturnDocument.AFTER
        )
        sub_cats = get_sub_cat(lnk)
        for sub_cat in sub_cats:
            sub_cat["parent"] = category_db["_id"]
            print(sub_cat)
            sub_category_db = db_categories.find_one_and_update({"link": sub_cat["link"]},
                {"$set": sub_cat},
                upsert=True,
                return_document=ReturnDocument.AFTER
            )

    #     categories.append({"name":name, "link":lnk, "subcategories":sub_cats, "rus_name": cat_link.string.strip()})

    # items[item]["categories"] = categories

print(items)

items = db_categories.find()


# cloth_nav = clothes_soup.find("ul", {"class": "js-outlet-icons-slider"})
# clothes = cloth_nav.find_all("a")

# for cloth in clothes:
#     link = cloth.attrs["href"].split("aim=")[1]
#     el = cloth.find("p", {"class":"outlet-slider-item__title"})
#     name = my_translit(el.string)
#     print(name, link)

#     clothes = requests.get(f"{base_link}/{link}/?page=1") #/c/369/clothes-platiya/?page=1")
#     clothes_soup = BeautifulSoup(clothes.content, "html.parser")

#     cloth_nav = clothes_soup.find("div", {"class": "products-catalog__list"})
#     clothes = cloth_nav.find_all("div", {"class": "products-list-item"})

# for cloth in clothes: 
#     images = cloth.attrs["data-gallery"].replace("[","").replace("]","").replace('"',"").split(",")
#     sku = cloth.attrs["data-sku"]
#     os.mkdir(f"temp/{sku}")
#     i = 1
#     for image in images:
#         url = f"http:{image.strip()}"
#         image = requests.get(url)
#         file = open(f"temp/{sku}/{i}.jpg", "wb")
#         file.write(image.content)
#         file.close()
#         i += 1
