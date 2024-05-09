"use client";

import {
    SignInButton,
    SignUpButton,
    SignedIn,
    SignedOut,
    UserButton,
} from "@clerk/nextjs";
import Link from "next/link";

export default function Navbar() {
    return (
        <header>
            <div className="flex flex-row grow justify-start px-8 py-4 text-2xl h-8">
                <div>
                    <Link href="/">Home</Link>
                </div>
                <SignedOut>
                    <div className="ml-auto">
                        <SignInButton />
                    </div>
                    <div className="ml-8">
                        <SignUpButton />
                    </div>
                </SignedOut>
                <SignedIn>
                    <div className="ml-8">
                        <Link href="/joined-discussions">
                            Joined Discussions
                        </Link>
                    </div>
                    <div className="ml-auto">
                        <Link href="/my-discussions">My Discussions</Link>
                    </div>
                    <div className="ml-8 pt-[3px]">
                        <UserButton afterSignOutUrl="/" />
                    </div>
                </SignedIn>
            </div>
        </header>
    );
}
