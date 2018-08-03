# Frequently Asked Questions


### Is this library thread-safe?

Yes, this library is fully thread-safe.


### What are the advantages compared to .NET events?

.NET events are nice. It's great to have a runtime that natively supports events. But there are also some real-world "problems" that his library tries to solve.

 - .NET events don't support *async* event handler. You can have *async void* handler that are invoked with fire-and-forget semantic, but you can't natively await completion of  async event handler.
 - .NET events don't invoke all remaining event handlers, when a previously invoked event handler threw an exception.
 - Removal of event handler is sometimes a bit hard, especially when you use lambda expressions a lot.
 - .NET events don't support weak subscriptions that are automatically removed after the event handler got garbage-collected.  


### What about Reactive Extensions?

[Rx.NET](http://reactivex.io/) is a great technology, but it's API can be a bit difficult to use with it's *OnNext()*, *OnCompleted()* and *OnError()* methods. Its strength lies in the processing and coordination of streams of events, not in simplicity.


### Who uses this library?

A few years ago, at my day job we started development of a new software platform for a next generation product family. We considered to use Rx.NET as replacement for all .NET events, because we weren't happy with some limitations of .NET events. But, Rx.NET also doesn't fit well with our requirements.

So, I started to experiment with a simple observer-pattern implementation on my own that would fulfill our requirements. I did that in my free time, but soon the library development was also partly done at my day job, which finally resulted in a closed-source solution.

From that time on, our closed-source event library was a cornerstone of our new software platform.

A few years later, I started to rewrote the entire library once again from scratch with the goal to make it open source. I wanted to share it with the community and also use it for my own personal projects.

That said, *Amarok.Events* isn't that widely used, but the concepts already have proven to work well.