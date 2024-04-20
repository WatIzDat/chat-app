import Link from "next/link";

export default function Navbar() {
    return (
        <div className="flex flex-row grow justify-start px-8 py-4 text-2xl">
            <Link className="ml-auto" href="/">
                Sign In
            </Link>
        </div>
    );
}
