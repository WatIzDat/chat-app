import { UserButton } from "@clerk/nextjs";
import Link from "next/link";
import { auth } from "@clerk/nextjs/server";

export default function Navbar() {
    const { userId } = auth();

    console.log(userId);

    return (
        <div className="flex flex-row grow justify-start px-8 py-4 text-2xl">
            {!userId && (
                <>
                    <Link className="ml-auto" href="/sign-in">
                        Sign In
                    </Link>
                    <Link className="ml-8" href="/sign-up">
                        Sign Up
                    </Link>
                </>
            )}
            {userId && (
                <div className="ml-auto">
                    <UserButton />
                </div>
            )}
        </div>
    );
}
