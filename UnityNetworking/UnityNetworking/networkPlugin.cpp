#include "pch.h"
#include "networkPlugin.h"

#include "foo.h"
#include <memory>

std::unique_ptr<Foo> inst = 0;
int initFoo(int f_new) {
	inst = std::make_unique<Foo>(f_new);
	return 1;
};

int doFoo(int bar) {
	inst->foo(bar);
	return 1;
}

int termFoo() {
	inst.reset();
	inst = 0;
	return 1;
}