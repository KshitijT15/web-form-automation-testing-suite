#pragma once
#include <string>
#include <vector>
using namespace std;

struct User {
    string name, email, phone, password;
    bool isValid;
    string failField; // "email", "name", "password", or ""
};