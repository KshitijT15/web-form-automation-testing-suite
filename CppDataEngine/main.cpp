#include "TestDataGenerator.h"

int main() {
    TestDataGenerator gen;
    gen.saveToJson(gen.generateAll(), "test_data.json");
    return 0;
}