import { fetchJoinedDiscussionsByUser } from "@/app/lib/data";
import { ArrowRightCircleIcon } from "@heroicons/react/24/solid";
import { redirect } from "next/navigation";

export default async function Page({ params }: { params: { id: string } }) {
    const joinedDiscussions: { id: string; name: string }[] =
        await fetchJoinedDiscussionsByUser();

    if (
        !joinedDiscussions ||
        !joinedDiscussions.map((d) => d.id).includes(params.id)
    ) {
        return redirect(`/join-discussion/${params.id}`);
    }

    return (
        <main className="h-full">
            <div className="flex flex-col items-center justify-end h-full">
                <form className="flex flex-row w-[calc(100vw-theme('spacing.32'))] mb-16 bg-zinc-900 rounded-3xl">
                    <input
                        className="bg-zinc-900 rounded-3xl flex-grow pl-4 pt-2 pb-2 pr-2"
                        type="text"
                        id="message"
                        name="message"
                        placeholder="Send a message..."
                    />

                    <button className="p-2 place-self-end" type="submit">
                        <ArrowRightCircleIcon className="size-12 text-zinc-500" />
                    </button>
                </form>
            </div>
        </main>
    );
}
