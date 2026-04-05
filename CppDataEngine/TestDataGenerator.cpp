#include "TestDataGenerator.h"
#include <fstream>
#include <iostream>
#include <ctime>
#include <cstdlib>

vector<string> names   = {"Rahul Sharma","Priya Patel","Amit Singh","Sneha Joshi"};
vector<string> domains = {"gmail.com","yahoo.com","outlook.com"};
vector<string> handles = {"rahul","priya","amit","sneha"};
vector<string> goodPass= {"Pass@1234","Secure#99","Hello@999"};
vector<string> badEmail= {"notanemail","missing@","@nodomain","no-at-sign"};

string rName()  { return names[rand()%names.size()]; }
string rPhone() { string p="9"; for(int i=0;i<9;i++) p+=to_string(rand()%10); return p; }
string rValidEmail() { return handles[rand()%handles.size()]+to_string(rand()%999)+"@"+domains[rand()%domains.size()]; }
string rBadEmail()   { return badEmail[rand()%badEmail.size()]; }
string rGoodPass()   { return goodPass[rand()%goodPass.size()]; }

vector<User> TestDataGenerator::generateAll() {
    srand((unsigned)time(0));
    vector<User> users;

    // 3 valid users
    for(int i=0;i<3;i++)
        users.push_back({rName(), rValidEmail(), rPhone(), rGoodPass(), true, ""});

    // 2 invalid email
    for(int i=0;i<2;i++)
        users.push_back({rName(), rBadEmail(), rPhone(), rGoodPass(), false, "email"});

    // 2 invalid password
    for(int i=0;i<2;i++)
        users.push_back({rName(), rValidEmail(), rPhone(), "123", false, "password"});

    // 1 empty name
    users.push_back({"", rValidEmail(), rPhone(), rGoodPass(), false, "name"});

    return users;
}

void TestDataGenerator::saveToJson(const vector<User>& users, const string& file) {
    ofstream f(file);
    f << "[\n";
    for(int i=0;i<(int)users.size();i++) {
        auto& u = users[i];
        f << "  {\n"
          << "    \"name\": \""      << u.name      << "\",\n"
          << "    \"email\": \""     << u.email     << "\",\n"
          << "    \"phone\": \""     << u.phone     << "\",\n"
          << "    \"password\": \""  << u.password  << "\",\n"
          << "    \"isValid\": "     << (u.isValid ? "true" : "false") << ",\n"
          << "    \"failField\": \"" << u.failField << "\"\n"
          << "  }" << (i<(int)users.size()-1 ? "," : "") << "\n";
    }
    f << "]\n";
    cout << "Generated " << users.size() << " test cases in " << file << "\n";
}