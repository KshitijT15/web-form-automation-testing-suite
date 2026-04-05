#pragma once
#include "User.h"

class TestDataGenerator {
public:
    vector<User> generateAll();
    void saveToJson(const vector<User>& users, const string& filename);
};