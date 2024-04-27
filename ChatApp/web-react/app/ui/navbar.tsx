"use client";

import {
    SignInButton,
    SignUpButton,
    SignedIn,
    SignedOut,
    UserButton,
} from "@clerk/nextjs";

export default function Navbar() {
    return (
        <header>
            <div className="flex flex-row grow justify-start px-8 py-4 text-2xl h-8">
                <SignedOut>
                    <div className="ml-auto">
                        <SignInButton />
                    </div>
                    <div className="ml-8">
                        <SignUpButton />
                    </div>
                </SignedOut>
                <SignedIn>
                    <div className="ml-auto">
                        <UserButton afterSignOutUrl="/" />
                    </div>
                </SignedIn>
            </div>
        </header>
    );
}
