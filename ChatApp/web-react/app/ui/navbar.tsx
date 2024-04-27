"use client";

import {
    SignInButton,
    SignUpButton,
    SignedIn,
    SignedOut,
    UserButton,
} from "@clerk/nextjs";
import { auth, currentUser } from "@clerk/nextjs/server";
import { fetchUserById } from "../lib/data";

export default async function Navbar() {
    // console.log(auth());

    // let isSignedIn = null;

    // try {
    //     // const user = await currentUser();
    //     const { userId } = auth();

    //     if (userId) {
    //         const user = await fetchUserById(userId);

    //         if (user === null) {
    //             isSignedIn = false;
    //         }
    //     }
    // } catch (error) {
    //     isSignedIn = false;
    // }

    return (
        <div className="flex flex-row grow justify-start px-8 py-4 text-2xl">
            <SignedOut>
                <SignInButton mode="modal" />
                <SignUpButton mode="modal" />
                <p>another test</p>
            </SignedOut>
            <SignedIn>
                <UserButton afterSignOutUrl="/" />
                <p>test</p>
            </SignedIn>
        </div>
    );
}
