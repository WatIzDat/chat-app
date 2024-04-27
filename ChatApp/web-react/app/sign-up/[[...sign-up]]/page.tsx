import { SignUp } from "@clerk/nextjs";

export default function Page() {
    return (
        <main className="h-full">
            <div className="flex items-center justify-center h-full">
                <SignUp />
            </div>
        </main>
    );
}
